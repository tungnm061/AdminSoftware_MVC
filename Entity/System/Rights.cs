namespace Entity.System
{
    public class Rights
    {
        public int UserId { get; set; }
        public int FunctionId { get; set; }
        public bool IsView { get; set; }
        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }

        #region function

        public string FunctionName { get; set; }
        public int FSortOrder { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }

        #endregion

        #region module

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string MSortOrder { get; set; }
        public string GroupName { get; set; }

        #endregion
    }
}