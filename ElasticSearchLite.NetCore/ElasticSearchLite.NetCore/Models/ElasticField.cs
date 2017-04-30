namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticField
    {
        private string _name;

        public string Name
        {
            get
            {
                if (_name == "Id")
                {
                    return ElasticFields.Id.Name;
                }
                return _name;
            }
            set { _name = value; }
        }
    }
}
