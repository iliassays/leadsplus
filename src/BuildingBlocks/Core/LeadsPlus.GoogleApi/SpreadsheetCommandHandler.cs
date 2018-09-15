﻿namespace LeadsPlus.GoogleApis.Command
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
        IRequestHandler<AssigSpreadsheetPermissionCommand, bool>
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

        public async Task<Spreadsheet> Handle(CreateSpreadsheetCommand createSpreadsheetCommand, CancellationToken cancellationToken)
        {
            this.sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = createSpreadsheetCommand.ApplicationName,
            });

            Spreadsheet spreadsheet = await CreateSpreadSheet(createSpreadsheetCommand.SpreadSheetName, createSpreadsheetCommand.WorkSheetName);

            await InsertHeader(spreadsheet.SpreadsheetId, createSpreadsheetCommand.WorkSheetName, createSpreadsheetCommand.HeaderValues);

            if (createSpreadsheetCommand.InitialValues.Count > 0)
            {
                await InsertFirstRow(spreadsheet.SpreadsheetId, createSpreadsheetCommand.WorkSheetName, createSpreadsheetCommand.InitialValues);
            }

            return spreadsheet;
        }
        
        public async Task<bool> Handle(AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionCommand, CancellationToken cancellationToken)
        {
            this.driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = assigSpreadsheetPermissionCommand.ApplicationName,
            });

            Permission userPermission = new Permission()
            {
                Type = "user",
                Role = "writer",
                EmailAddress = assigSpreadsheetPermissionCommand.Email
            };

            await driveService.Permissions.Create(userPermission, assigSpreadsheetPermissionCommand.SpreadsheetId).ExecuteAsync();

            return true;
        }

        private async Task<Spreadsheet> CreateSpreadSheet(string spreadSheetName, string worksheetName)
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
                rangeEnd = rangeStart++;
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

        private async Task<bool> InsertFirstRow(string spreadSheetId, string workSheetName, IList<object> initialValues)
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