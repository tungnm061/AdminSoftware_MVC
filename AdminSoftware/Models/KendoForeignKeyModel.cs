namespace AdminSoftware.Models
{
    public class KendoForeignKeyModel
    {
        // ReSharper disable once InconsistentNaming
        public string value { get; set; }
        // ReSharper disable once InconsistentNaming
        public string text { get; set; }
        // ReSharper disable once InconsistentNaming
        public string othor { get; set; }
        // ReSharper disable once InconsistentNaming
        public bool locked { get; set; }

        public KendoForeignKeyModel(string text, string value,string othor = null,bool locked = false)
        {
            this.value = value;
            this.text = text;
            this.othor = othor;
            this.locked = locked;
        }
        public KendoForeignKeyModel() { }
    }
}