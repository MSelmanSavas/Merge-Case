using UsefulDataTypes;

namespace MergeCase.Entities
{
    [System.Serializable]
    public class EntityComponentDictionary : SerializableReferenceDictionary<TypeReferenceInheritedFrom<IComponent>, IComponent>
    {

    }
}
