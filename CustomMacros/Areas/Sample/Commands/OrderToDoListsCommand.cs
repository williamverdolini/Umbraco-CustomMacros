using CustomMacros.Areas.Infrastructure.Commands;

namespace CustomMacros.Areas.Sample.Commands
{
    public class OrderToDoListsCommand : Command
    {
        private const string ORDER_FIELD_NAME = "FieldName";
        private const string ORDER_IS_ASCENDING = "IsAscending";

        public string FieldName
        {
            get { return props[ORDER_FIELD_NAME]; }
            set
            {
                if (props.ContainsKey(ORDER_FIELD_NAME)) props[ORDER_FIELD_NAME] = value;
                else props.Add(ORDER_FIELD_NAME, value);
            }
        }

        public string IsAscending
        {
            get { return props[ORDER_IS_ASCENDING]; }
            set
            {
                if (props.ContainsKey(ORDER_IS_ASCENDING)) props[ORDER_IS_ASCENDING] = value;
                else props.Add(ORDER_IS_ASCENDING, value);
            }
        }
    }
}