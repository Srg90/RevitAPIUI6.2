using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPIUILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Mechanical;

namespace RevitAPIUI6
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; }
        public List<FamilySymbol> FurnTypes { get; } = new List<FamilySymbol>();
        public List<Level> FurnLevels { get; } = new List<Level>();
        public FamilySymbol SelectedFurnType { get; set; }
        public Level SelectedLevel { get; set; }
        public List<XYZ> Points { get; } = new List<XYZ>();

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            FurnTypes = FurnitureUtils.GetFurnitureTypes(commandData);
            FurnLevels = LevelsUtils.GetLevels(commandData);
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точку", ObjectSnapTypes.Endpoints);
        }

        private void OnSaveCommand()
        {
            //RaiseHideRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points.Count < 1 || SelectedFurnType == null || SelectedLevel == null)
                return;


                    FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurnType, Points[0], SelectedLevel);

            RaiseCloseRequest();
            //RaiseShowRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        //public event EventHandler HideRequest;
        //private void RaiseHideRequest()
        //{
        //    HideRequest?.Invoke(this, EventArgs.Empty);
        //}

        //public event EventHandler ShowRequest;
        //private void RaiseShowRequest()
        //{
        //    ShowRequest?.Invoke(this, EventArgs.Empty);
        //}
    }
}
