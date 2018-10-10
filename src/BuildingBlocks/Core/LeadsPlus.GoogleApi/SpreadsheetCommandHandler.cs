namespace LeadsPlus.GoogleApis.Command
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Drive.v3.Data;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using MediatR;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SpreadsheetCommandHandler
        : IRequestHandler<CreateSpreadsheetCommand, Spreadsheet>,
        IRequestHandler<AssigSpreadsheetPermissionCommand, bool>,
        IRequestHandler<InsertRowToSpreadsheetCommand, Spreadsheet>
    {
        private IGoogleApiConnector googleApiConnector;
        private readonly IMediator mediator;

        private SheetsService sheetsService;
        private DriveService driveService;
        private GoogleCredential googleCredential;

        public SpreadsheetCommandHandler(IMediator mediator,
            IGoogleApiConnector googleApiConnector)
        {
            this.googleApiConnector = googleApiConnector ?? throw new ArgumentNullException(nameof(googleApiConnector));
            this.googleCredential = this.googleApiConnector.CreateCredential("LeadsPlus-7d818303309c.json", new string[] { SheetsService.Scope.Spreadsheets, SheetsService.Scope.Drive });
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Spreadsheet> Handle(CreateSpreadsheetCommand @command, CancellationToken cancellationToken)
        {
            this.sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = @command.ApplicationName
            });

            Spreadsheet spreadsheet = await CreateSpreadsheet(@command.SpreadSheetName, @command.WorkSheetName);

            await InsertHeader(spreadsheet.SpreadsheetId, @command.WorkSheetName, @command.HeaderValues);

            if (@command.InitialValues.Count > 0)
            {
                await InsertRow(spreadsheet.SpreadsheetId, @command.WorkSheetName, @command.InitialValues);
            }

            return spreadsheet;
        }

        public async Task<Spreadsheet> Handle(InsertRowToSpreadsheetCommand @command, CancellationToken cancellationToken)
        {
            this.sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = @command.ApplicationName,
            });

            Spreadsheet spreadsheet = await GetSpreadsheet(@command.SpreadSheetId);

            //await InsertHeader(spreadsheet.SpreadsheetId, updateSpreadsheetCommand.WorkSheetName, updateSpreadsheetCommand.HeaderValues);

            if (@command.Values.Count > 0)
            {
                await InsertRow(spreadsheet.SpreadsheetId, @command.WorkSheetName, @command.Values);
            }

            return spreadsheet;
        }

        public async Task<bool> Handle(AssigSpreadsheetPermissionCommand @command, CancellationToken cancellationToken)
        {
            this.sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = @command.ApplicationName,
            });

            this.driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = @command.ApplicationName,
            });

            Permission userPermission = new Permission()
            {
                Type = "user",
                Role = "writer",
                EmailAddress = @command.Email
            };

            await driveService.Permissions.Create(userPermission, @command.SpreadsheetId).ExecuteAsync();

            return true;
        }

        private async Task<Spreadsheet> CreateSpreadsheet(string spreadSheetName, string worksheetName)
        {
            Spreadsheet createRequest = new Spreadsheet
            {
                Properties = new SpreadsheetProperties
                {
                    Title = spreadSheetName
                }
            };

            createRequest.Sheets = new List<Sheet>
            {
                new Sheet
                {
                    Properties = new SheetProperties
                    {
                        Title = worksheetName
                    }
                }
            };

            var request = sheetsService.Spreadsheets.Create(createRequest);
            var response = await request.ExecuteAsync();

            return response;
        }

        private async Task<Spreadsheet> GetSpreadsheet(string spreadSheetId)
        {
            var request = sheetsService.Spreadsheets.Get(spreadSheetId);
            var response = await request.ExecuteAsync();

            return response;
        }

        private async Task InsertHeader(string spreadSheetId, string workSheetName, IList<object> headerValues)
        {
            var range = CreateRange(workSheetName, headerValues.Count);

            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { headerValues }
            };

            var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadSheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var appendReponse = await appendRequest.ExecuteAsync();

            await HighLightHeader(spreadSheetId, workSheetName);
        }

        private string CreateRange(string sheet, int totalColumn)
        {
            char rangeStart = 'A';
            char rangeEnd = 'A';

            int totValues = totalColumn;

            for (int i = 0; i < totValues; i++)
            {
                rangeEnd++;
            }

            return $"{sheet}!{rangeStart}:{rangeEnd}";
        }

        private async Task<bool> HighLightHeader(string spreadSheetId, string workSheetName)
        {
            Spreadsheet spr = sheetsService.Spreadsheets.Get(spreadSheetId).Execute();
            Sheet sh = spr.Sheets.Where(s => s.Properties.Title == workSheetName).FirstOrDefault();
            int sheetId = (int) sh.Properties.SheetId;

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

            var bur = sheetsService.Spreadsheets.BatchUpdate(bussr, spreadSheetId);
            await bur.ExecuteAsync();

            return true;
        }

        private async Task<bool> InsertRow(string spreadSheetId, string workSheetName, IList<object> initialValues)
        {
            string sheet = workSheetName;
            var range = CreateRange(workSheetName, initialValues.Count);

            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { initialValues }
            };

            var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadSheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = await appendRequest.ExecuteAsync();

            return true;
        }
    }
}