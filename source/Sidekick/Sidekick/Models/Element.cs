namespace Sidekick.Models
{
    public class Element : IElement
    {
        public Element(string id, string number, string text)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Number = number ?? throw new System.ArgumentNullException(nameof(number));
            Text = text ?? throw new System.ArgumentNullException(nameof(text));
        }

        public string Id { get; set; }
        public string Number { get; set; }
        public string Text { get; }
    }
}
