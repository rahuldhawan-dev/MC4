using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCallImporter.MappingAnalysis.Models;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCallImporter.MappingAnalysis
{
    public class ExcelRecordTestParser
    {
        #region Private Methods

        private static void ParseMappingsFromClass(ClassDeclarationSyntax cls, ExcelToMapCallMapping mapping)
        {
            var methods = cls.Members.OfType<MethodDeclarationSyntax>();
            TryParseInnerTestMappings(methods, mapping);
            TryParseOtherTests(methods, mapping);
        }

        private void ParseMappingsFromClass(ClassDeclarationSyntax cls, ExcelToMapCallMapping equipmentBaseMappings, ExcelToMapCallEquipmentMapping mapping)
        {
            mapping.SimpleMappings.AddRange(equipmentBaseMappings.SimpleMappings);
            mapping.OtherTests.AddRange(equipmentBaseMappings.OtherTests);

            var methods = cls.Members.OfType<MethodDeclarationSyntax>();
            TryParseTestCharacteristicMappings(methods, mapping);
            ParseMappingsFromClass(cls, mapping);
        }

        private static void TryParseOtherTests(IEnumerable<MethodDeclarationSyntax> methods, ExcelToMapCallMapping mapping)
        {
            var tests = methods.Where(m =>
                m.Modifiers.Any(mod => mod.Text == "public") &&
                m.Identifier.ValueText.StartsWith("Test") &&
                !new[] {"TestInitialize", "TestCleanup", "TestMappings"}.Contains(m.Identifier.ValueText));

            foreach (var test in tests)
            {
                if (new[] {"MapToEntity", "Validate", "Sets", "Mapping"}.Any(s => test.Identifier.ValueText.Contains(s)))
                {
                    throw new InvalidOperationException($"Bad test name: {mapping.ExcelModelName} - '{test}'");
                }

                mapping.AddOtherTest(test);
            }
        }

        private static void TryParseSimpleMappings(IEnumerable<MethodDeclarationSyntax> methods, string testMethodName, Action<InvocationExpressionSyntax> addMapping)
        {
            MethodDeclarationSyntax moc;

            if ((moc = methods.SingleOrDefault(m => m.Identifier.ValueText == testMethodName)) == null)
            {
                return;
            }

            foreach (var statement in moc.Body.Statements)
            {
                if (statement.Kind() != SyntaxKind.ExpressionStatement)
                {
                    throw new InvalidOperationException(
                        $"Not sure what to do about statement of kind {statement.Kind()} in test method {testMethodName}");
                }

                var expr = (statement as ExpressionStatementSyntax)?.Expression as InvocationExpressionSyntax;

                addMapping(expr);
            }
        }

        private static void TryParseTestCharacteristicMappings(IEnumerable<MethodDeclarationSyntax> methods, ExcelToMapCallEquipmentMapping mapping)
        {
            TryParseSimpleMappings(methods, "TestCharacteristicMappings", mapping.AddEquipmentCharacteristicMapping);
        }

        private static void TryParseInnerTestMappings(IEnumerable<MethodDeclarationSyntax> methods, ExcelToMapCallMapping mapping)
        {
            TryParseSimpleMappings(methods, "InnerTestMappings", expr => {
                if (expr.Expression.ToString() != "WithCharacteristicMappingTester")
                {
                    mapping.AddSimpleMapping(expr);
                }
            });
        }

        private static ClassDeclarationSyntax ParseClassFromFile(FileSystemInfo file)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file.FullName), path: file.FullName);

            if (!(tree.GetRoot() is CompilationUnitSyntax root) ||
                !(root.Members[0] is NamespaceDeclarationSyntax ns) ||
                !(ns.Members[0] is ClassDeclarationSyntax cls))
            {
                throw new ArgumentException($"Could not find test class definition in file {file.FullName}");
            }

            return cls;
        }

        protected TMapping ParseThing<TMapping>(FileSystemInfo file, Action<ClassDeclarationSyntax, TMapping> doMapping)
            where TMapping : ExcelToMapCallMapping
        {
            var cls = ParseClassFromFile(file);
            var mapping = (TMapping)Activator.CreateInstance(typeof(TMapping), cls);

            doMapping(cls, mapping);

            return mapping;
        }

        #endregion

        #region Exposed Methods

        public ExcelToMapCallMapping Parse(FileSystemInfo file)
        {
            return ParseThing<ExcelToMapCallMapping>(file, ParseMappingsFromClass);
        }

        public ExcelToMapCallEquipmentMapping ParseEquipment(ExcelToMapCallMapping equipmentBaseMappings, FileInfo file)
        {
            return ParseThing<ExcelToMapCallEquipmentMapping>(file,
                (cls, mapping) => ParseMappingsFromClass(cls, equipmentBaseMappings, mapping));
        }

        #endregion
    }
}