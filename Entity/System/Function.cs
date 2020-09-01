namespace Entity.System
{
    public class Function
    {
        public int FunctionId { get; set; }
        public int ModuleId { get; set; }
        public string FunctionName { get; set; }
        public int SortOrder { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }
    }
}