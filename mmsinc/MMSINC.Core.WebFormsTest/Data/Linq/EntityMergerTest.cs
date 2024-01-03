using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.Linq;

namespace MMSINC.Core.WebFormsTest.Data.Linq
{
    /// <summary>
    /// Summary description for EntityMergerTest
    /// </summary>
    [TestClass]
    public class EntityMergerTest
    {
        [TestMethod]
        public void TestMerge()
        {
            DateTime now = DateTime.Now,
                     twentyYearsAgo = DateTime.Now.SubtractYears(20);
            TestMergeObject source = new TestMergeObject {
                                // represents the new TestMergeObject object that has been created
                                // and submitted by whatever web control, ready to update
                                // the actual persisted object
                                TestMergeObjectID = 1,
                                FirstString = "Joe",
                                SecondString = "Smith",
                            },
                            dest = new TestMergeObject {
                                // represents the TestMergeObject object which already existed
                                // in the database
                                TestMergeObjectID = 1,
                                FirstString = "John",
                                SecondString = "Smith",
                                ThirdString = "14 Elm Street",
                                NullableDate = twentyYearsAgo,
                                Date = now
                            };
            EntityMerger.Merge(ref dest, source);

            Assert.AreNotSame(source, dest);
            Assert.AreEqual(source.FirstString, dest.FirstString);
            Assert.AreEqual(source.SecondString, dest.SecondString);
            Assert.AreEqual(now, dest.Date);

            Assert.IsNotNull(dest.ThirdString);
            Assert.IsNotNull(dest.NullableDate);
            Assert.AreEqual(twentyYearsAgo, dest.NullableDate.Value);
        }

        [TestMethod]
        public void TestMergeDoesNotOverwriteWithNullValues()
        {
            var expectedID = 1;
            var refObj = new TestReferenceObject {
                TestReferenceObjectID = expectedID
            };
            const string firstString = "First String",
                         secondString = "Second String";
            TestMergeObject source = new TestMergeObject {
                                FirstString = firstString
                            },
                            dest = new TestMergeObject {
                                SecondString = secondString,
                                ReferenceObject = refObj
                            };

            EntityMerger.Merge(ref dest, source);

            Assert.AreEqual(firstString, dest.FirstString);
            Assert.AreEqual(secondString, dest.SecondString);
            Assert.AreSame(refObj, dest.ReferenceObject);
            Assert.AreEqual(expectedID, dest.ReferenceObjectID);
        }

        [TestMethod]
        public void TestMergeDoesNotOverwriteWithNullValuesWhenMergeNullsIsFalse()
        {
            var expectedID = 1;
            var refObj = new TestReferenceObject {
                TestReferenceObjectID = expectedID
            };
            const string firstString = "First String",
                         secondString = "Second String";
            TestMergeObject source = new TestMergeObject {
                                FirstString = firstString
                            },
                            dest = new TestMergeObject {
                                SecondString = secondString,
                                ReferenceObject = refObj
                            };

            EntityMerger.Merge(ref dest, source, false);

            Assert.AreEqual(firstString, dest.FirstString);
            Assert.AreEqual(secondString, dest.SecondString);
            Assert.AreSame(refObj, dest.ReferenceObject);
            Assert.AreEqual(expectedID, dest.ReferenceObjectID);
        }

        [TestMethod]
        public void TestMergeOverwritesWithNullValuesWhenMergeNullsIsTrueWithoutAssociations()
        {
            var expectedID = 1;
            var refObj = new TestReferenceObject {
                TestReferenceObjectID = expectedID
            };
            const string firstString = "First String",
                         secondString = "Second String";
            TestMergeObject source = new TestMergeObject {
                                SecondString = secondString,
                            },
                            dest = new TestMergeObject {
                                FirstString = firstString,
                            };

            EntityMerger.Merge(ref dest, source, true);

            Assert.IsNull(dest.FirstString);
            Assert.AreEqual(secondString, dest.SecondString);
        }
    }

    internal class TestMergeObject
    {
        #region Private Members

        private int? _referenceObjectID;
        private EntityRef<TestReferenceObject> _referenceObject;
        private EntitySet<TestReferenceObject> _referenceObjects;

        #endregion

        #region Properties

        [Column(IsPrimaryKey = true)]
        public int TestMergeObjectID { get; set; }

        [Column]
        public string FirstString { get; set; }

        [Column]
        public string SecondString { get; set; }

        [Column]
        public string ThirdString { get; set; }

        [Column]
        public DateTime? NullableDate { get; set; }

        [Column]
        public DateTime Date { get; set; }

        [Column]
        public int? ReferenceObjectID
        {
            get { return _referenceObjectID; }
            set
            {
                if (_referenceObjectID != value)
                {
                    _referenceObjectID = value;
                }
            }
        }

        [Association(IsForeignKey = true)]
        public TestReferenceObject ReferenceObject
        {
            get { return _referenceObject.Entity; }
            set
            {
                var previousValue = _referenceObject.Entity;
                if (previousValue != value || !_referenceObject.HasLoadedOrAssignedValue)
                {
                    if (previousValue != null)
                    {
                        _referenceObject.Entity = null;
                        previousValue.Parent = null;
                    }

                    _referenceObject.Entity = value;
                    if (value != null)
                    {
                        value.Parent = this;
                        _referenceObjectID = value.TestReferenceObjectID;
                    }
                    else
                        _referenceObjectID = default(int?);
                }
            }
        }

        [Association]
        public EntitySet<TestReferenceObject> ReferenceObjects
        {
            get { return _referenceObjects; }
            set { _referenceObjects.Assign(value); }
        }

        #endregion

        #region Constructors

        public TestMergeObject()
        {
            _referenceObject = default(EntityRef<TestReferenceObject>);
            _referenceObjects =
                new EntitySet<TestReferenceObject>(attach_ReferenceObjects,
                    detach_ReferenceObjects);
        }

        #endregion

        #region Private Methods

        private void attach_ReferenceObjects(TestReferenceObject obj)
        {
            obj.Parent = this;
        }

        private void detach_ReferenceObjects(TestReferenceObject obj)
        {
            obj.Parent = null;
        }

        #endregion
    }

    internal class TestReferenceObject
    {
        #region Private Members

        private int _testReferenceObjectID;
        private int? _parentID;
        private EntityRef<TestMergeObject> _parent;

        #endregion

        #region Properties

        [Column(IsPrimaryKey = true)]
        public int TestReferenceObjectID
        {
            get { return _testReferenceObjectID; }
            set
            {
                if (_testReferenceObjectID != value)
                {
                    _testReferenceObjectID = value;
                }
            }
        }

        [Column]
        public int? ParentID
        {
            get { return _parentID; }
            set
            {
                if (_parentID != value)
                {
                    _parentID = value;
                }
            }
        }

        [Association]
        public TestMergeObject Parent
        {
            get { return _parent.Entity; }
            set
            {
                var previousValue = _parent.Entity;
                if (previousValue != value || !_parent.HasLoadedOrAssignedValue)
                {
                    if (previousValue != null)
                    {
                        _parent.Entity = null;
                        previousValue.ReferenceObjects.Remove(this);
                    }

                    _parent.Entity = value;
                    if (value != null)
                    {
                        value.ReferenceObjects.Add(this);
                        _parentID = value.TestMergeObjectID;
                    }
                    else
                        _parentID = default(int);
                }
            }
        }

        #endregion

        #region Constructors

        public TestReferenceObject()
        {
            _parent = default(EntityRef<TestMergeObject>);
        }

        #endregion
    }
}
