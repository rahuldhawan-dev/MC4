using System.Collections.Generic;
using MMSINC.Utilities.ObjectMapping;

// ReSharper disable CheckNamespace
namespace MMSINC.CoreTest.Utilities.ObjectMapping.ObjectMappingTestClasses
    // ReSharper restore CheckNamespace
{
    public class PrimaryObject
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public bool BoolProp { get; set; }
        public object PropertyNotOnSecondary { get; set; }
        public bool ReadOnlyProp { get; private set; }
        public List<string> ListPropWithDifferentType { get; set; }
        public int? NullableIntOnPrimaryIntOnSecondary { get; set; }
        public int IntOnPrimaryNullableIntOnSecondary { get; set; }
        public BaseClass ClassProp { get; set; }
        public DerivedBaseClass SubClassProp { get; set; }
        public BaseClass DifferentClassProp { get; set; }

        public string MappedBothWaysProp { get; set; }
        public string MappedToPrimaryProp { get; set; }
        public string MappedToSecondaryProp { get; set; }
        public string MappedNoneProp { get; set; }

        public object SecondaryWithNonPublicProperty { get; set; }

        public void SetReadOnlyProp(bool val)
        {
            ReadOnlyProp = val;
        }
    }

    public class SecondaryObject
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public object PropertyNotOnPrimary { get; set; }
        public bool ReadOnlyProp { get; private set; }
        public List<int> ListPropWithDifferentType { get; set; }
        public int NullableIntOnPrimaryIntOnSecondary { get; set; }
        public int? IntOnPrimaryNullableIntOnSecondary { get; set; }
        public DerivedBaseClass ClassProp { get; set; }
        public BaseClass SubClassProp { get; set; }
        public DifferentClass DifferentClassProp { get; set; }

        [AutoMap]
        public string MappedBothWaysProp { get; set; }

        [AutoMap(MapDirections.ToPrimary)]
        public string MappedToPrimaryProp { get; set; }

        [AutoMap(MapDirections.ToSecondary)]
        public string MappedToSecondaryProp { get; set; }

        [AutoMap(MapDirections.None)]
        public string MappedNoneProp { get; set; }

        internal object SecondaryWithNonPublicProperty { get; set; }

        public void SetReadOnlyProp(bool val)
        {
            ReadOnlyProp = val;
        }
    }

    public class BaseClass { }

    public class DerivedBaseClass : BaseClass { }

    public class DifferentClass { }

    #region Entity/ViewModel classes with MapAttributes

    public class MapAttributedSecondaryObject
    {
        public string BaseMapToPrimaryProperty { get; set; }
        public string BaseMapToSecondaryProperty { get; set; }
        public string VirtualMappedProperty { get; set; }
        public string OverrideMappedProperty { get; set; }
    }

    public abstract class MapAttributedPrimaryObject
    {
        [AutoMap(MapDirections.ToPrimary)]
        public string BaseMapToPrimaryProperty { get; set; }

        [AutoMap(MapDirections.ToSecondary)]
        public string BaseMapToSecondaryProperty { get; set; }

        [AutoMap(MapDirections.BothWays)]
        public virtual string VirtualMappedProperty { get; set; }

        [AutoMap(MapDirections.None)]
        public virtual string OverrideMappedProperty { get; set; }
    }

    public class DerivedMapAttributedPrimaryObject : MapAttributedPrimaryObject
    {
        // ReSharper disable RedundantOverridenMember
        public override string VirtualMappedProperty
        {
            get { return base.VirtualMappedProperty; }
            set { base.VirtualMappedProperty = value; }
        }
        // ReSharper restore RedundantOverridenMember

        [AutoMap(MapDirections.BothWays)]
        public override string OverrideMappedProperty
        {
            get { return base.OverrideMappedProperty; }
            set { base.OverrideMappedProperty = value; }
        }
    }

    #endregion

    //public class PrimaryModifierObject
    //{
    //    public object PrimaryPublicGetterSetterSecondaryPublicGetterSetter { get; set; }
    //    public object PrimaryPublicSetterSecondaryNonPublicGetterSetter { get; set; }
    //}

    //public class SecondaryModifierObject
    //{
    //    public object PrimaryPublicGetterSetterSecondaryPublicGetterSetter { get; set; }
    //    internal object PrimaryPublicSetterSecondaryNonPublicGetterSetter { get; set; }
    //}
}
