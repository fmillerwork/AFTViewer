

namespace AFTViewer.Model
{
    public class FailedAssertResult : FailureModel
    {
        public AssertType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public FailedAssertResult(AssertType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;
        }

        public enum AssertType
        {
            Text,
            Editability,
            Checked,
            ElementPresent,
            SelectedValue,
            SelectedLabel,
            Title,
            Value,
            //Alert,
            //Prompt,
            //Confirmation,
        }
    }
}
