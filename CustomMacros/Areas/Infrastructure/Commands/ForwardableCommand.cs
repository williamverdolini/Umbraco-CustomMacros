
namespace CustomMacros.Areas.Infrastructure.Commands
{
    public class ForwardableCommand : Command
    {
        private const string FORWARD_PAGE = "ForwardPage";
        private const string POPUP_OPTIONS = "PopupOptions";

        public string ForwardPage
        {
            get { return props[FORWARD_PAGE]; }
            set
            {
                if (props.ContainsKey(FORWARD_PAGE)) props[FORWARD_PAGE] = value;
                else props.Add(FORWARD_PAGE, value);
            }
        }

        public string PopupOptions
        {
            get { return props[POPUP_OPTIONS]; }
            set
            {
                if (props.ContainsKey(POPUP_OPTIONS)) props[POPUP_OPTIONS] = value;
                else props.Add(POPUP_OPTIONS, value);
            }
        }

        public ForwardableCommand() : base() { }
    }
}