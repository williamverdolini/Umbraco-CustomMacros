using CustomMacros.Areas.Infrastructure.Commands;

namespace CustomMacros.Areas.Sample.Commands
{
    public class SelectToDoListCommand : ForwardableCommand
    {
        private const string TODO_LIST_ID = "ToDoListId";

        public string ToDoListId
        {
            get { return props[TODO_LIST_ID]; }
            set
            {
                if (props.ContainsKey(TODO_LIST_ID)) props[TODO_LIST_ID] = value;
                else props.Add(TODO_LIST_ID, value);
            }
        }
    }
}