namespace JO.Data.Base.ValueObjects.Types
{
    public class JOTitle : ValueObject<JOTitle>
    {

        public string? Title { get; protected set; }
        public string? Name { get; protected set; }
        public string? Description { get; protected set; }

        public override void Validate()
        {
            //throw new NotImplementedException();
        }

        public JOTitle(string title)
        {
            Title = title;
            Name = title.Replace(" ", "_");

            Validate();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Name;
            yield return Description;
        }

        protected override void Update(JOTitle other)
        {
            Title = other.Title;
            Name = (other.Name ?? "").Replace(" ", "_");
            Description = other.Description;

            Validate();
        }

        protected override void Update(object parameter)
        {
            if (parameter is string)
            {
                Title = parameter.ToString();
                Name = parameter.ToString().Replace(" ", "_");
            }

            Validate();
        }

        [Obsolete]
        protected override void Update(object parameter, object parameter2)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        protected override void Update(object parameter, object parameter2, object parameter3)
        {
            throw new NotImplementedException();
        }
    }
}
