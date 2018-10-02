using Google.Apis.Sheets.v4.Data;
using MediatR;
using System.Collections.Generic;

namespace LeadsPlus.GoogleApis.Command
{
    public class CreateSpreadsheetCommand : IRequest<Spreadsheet>
    {
        public string ApplicationName { get; set; }
       
        public string SpreadSheetName { get; set; }
        public string WorkSheetName { get; set; }
        public IList<object> HeaderValues { get; set; }
        public IList<object> InitialValues { get; set; }
        public string Webroot { get; set; }

        public CreateSpreadsheetCommand()
        {
            HeaderValues = new List<object>();
            InitialValues = new List<object>();
        }
    }

    public class InsertRowToSpreadsheetCommand : IRequest<Spreadsheet>
    {
        public string ApplicationName { get; set; }
        public string SpreadSheetId { get; set; }
        public string WorkSheetName { get; set; }
        public IList<object> Values { get; set; }

        public InsertRowToSpreadsheetCommand()
        {
            Values = new List<object>();
        }
    }

    public class AssigSpreadsheetPermissionCommand : IRequest<bool>
    {
        public string ApplicationName { get; set; }
        public string Email { get; set; }
        public string SpreadsheetId { get; set; }
    }
}
