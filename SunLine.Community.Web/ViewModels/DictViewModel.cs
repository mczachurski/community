namespace SunLine.Community.Web.ViewModels
{
    public class DictViewModel<T>
    {
        public DictViewModel()
        { 
        }

        public DictViewModel(T id, string name)
        {
            Id = id;
            Name = name;
        }

        public T Id { get; set; }
        public string Name { get; set; }
    }
}

