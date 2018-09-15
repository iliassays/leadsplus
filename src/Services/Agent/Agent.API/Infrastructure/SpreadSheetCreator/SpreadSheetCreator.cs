using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Agent.SpreadSheetIntegration
{

    public class SpreadSheetCreator
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets, SheetsService.Scope.Drive };
        static string ApplicationName = "agent sheet";

        private UserCredential _credential;
        private SheetsService sheetsService;
        private DriveService driveService;
        private string _spreadSheetId;
        private string _range;

        private readonly string _spreadSheetName;
        private readonly string _workSheetName;
        private readonly IList<object> _headerValues;
        private readonly IList<object> _initialValues;

        public SpreadSheetCreator(SpreadSheetCreateOptions options)
        {
            _spreadSheetName = options.SpreadSheetName;
            _workSheetName = options.WorkSheetName;
            _headerValues = options.HeaderValues;
            _initialValues = options.InitialValues;
        }

        public Spreadsheet Create()
        {
            //Load creds
            LoadCredentials();

            //Create service
            CreateSheetsService();
            CreateDriveService();

            // TODO : Check if spreadsheet exists
            //Create spreadshit and work sheet
            Spreadsheet spreadsheet = CreateSpreadSheet(_spreadSheetName, _workSheetName);

            // TODO : Handle errors
            //InsertHeader
            InsertHeader();

            //TODO : Validate values
            //Inser first row
            InsertFirstRow();

            return spreadsheet;
        }

        private void CreateSheetsService() => sheetsService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = _credential,
            ApplicationName = ApplicationName,
        });

        private void CreateDriveService() => driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = _credential,
            ApplicationName = ApplicationName,
        });

        private void LoadCredentials()
        {
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
        }

        private Spreadsheet CreateSpreadSheet(string spreadSheetName, string worksheetName)
        {
            Spreadsheet createRequest = new Spreadsheet
            {
                Properties = new SpreadsheetProperties
                {
                    //NOTE : Name using agent's identity 
                    Title = spreadSheetName
                }
            };

            var sheet = new Sheet
            {
                Properties = new SheetProperties
                {
                    //NOTE : This will follow type form name
                    Title = worksheetName
                }
            };

            createRequest.Sheets = new List<Sheet>
            {
                sheet
            };

            var request = sheetsService.Spreadsheets.Create(createRequest);
            var response = request.Execute();
            
            _spreadSheetId = response.SpreadsheetId;

            return response;
        }

        public void CreateSpreadSheetPermission(string spreadSheetId, string emailAddress)
        {
            Permission userPermission = new Permission()
            {
                Type = "user",
                Role = "writer",
                EmailAddress = emailAddress
            };

            driveService.Permissions.Create(userPermission, spreadSheetId).Execute();
        }

        private void InsertHeader()
        {
            string sheet = _workSheetName;

            CreateRange(sheet);

            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { _headerValues }
            };

            var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, _spreadSheetId, _range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var appendReponse = appendRequest.Execute();            

            HighLightHeader();
        }

        private void CreateRange(string sheet)
        {
            char rangeStart = 'A';
            char rangeEnd = 'A';
            int totValues = _headerValues.Count;

            for (int i = 0; i < totValues; i++)
            {
                rangeEnd = rangeStart++;
            }

            _range = $"{sheet}!{rangeStart}:{rangeEnd}";
        }

        private void HighLightHeader()
        {
            Spreadsheet spr = sheetsService.Spreadsheets.Get(_spreadSheetId).Execute();
            Sheet sh = spr.Sheets.Where(s => s.Properties.Title == _workSheetName).FirstOrDefault();
            int sheetId = (int)sh.Properties.SheetId;

            //define cell color
            var userEnteredFormat = new CellFormat()
            {

                TextFormat = new TextFormat()
                {
                    Bold = true
                }
            };

            BatchUpdateSpreadsheetRequest bussr = new BatchUpdateSpreadsheetRequest();

            //create the update request for cells from the first row
            var updateCellsRequest = new Request()
            {
                RepeatCell = new RepeatCellRequest()
                {
                    Range = new GridRange()
                    {
                        SheetId = sheetId,
                        StartColumnIndex = 0,
                        StartRowIndex = 0,
                        EndColumnIndex = 28,
                        EndRowIndex = 1
                    },
                    Cell = new CellData()
                    {
                        UserEnteredFormat = userEnteredFormat
                    },
                    Fields = "UserEnteredFormat(TextFormat)"
                }
            };

            bussr.Requests = new List<Request>
            {
                updateCellsRequest
            };

            var bur = sheetsService.Spreadsheets.BatchUpdate(bussr, _spreadSheetId);
            bur.Execute();

        }

        private void InsertFirstRow()
        {
            string sheet = _workSheetName;

            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { _initialValues }
            };

            var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, _spreadSheetId, _range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }
    }
}
