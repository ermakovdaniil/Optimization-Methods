﻿using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog;
using View.Utils.IOService;
using View.Utils.MainWindowControlChanger;
using View.Utils.MessageBoxService;
using View.Utils.UserService;

namespace View.UserInterface.Technologist;

public class TechnologistControlVM : ViewModelBase
{
    private object _data;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly NavigationManager _navigationManager;
    private readonly IUserService _userService;

    #region Functions

    public TechnologistControlVM(UserDBContext userContext,
                                 MethodDBContext methodsContext,
                                 NavigationManager navigationManager,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IUserService userService)
    {
        _userContext = userContext;
        _methodsContext = methodsContext;
        _methodsContext.Methods.Load();
        _navigationManager = navigationManager;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        _userService = userService;
    }

    //private string CreateSearchResult(Result AnalysisResult)
    //{
    //    string searchResult;
    //    searchResult = AnalysisResult.AnRes + "\n" +
    //                   "Дата проведения анализа: " + AnalysisResult.Date + "\n" +
    //                   "Время проведения: " + AnalysisResult.Time + " мс\n" +
    //                   "Процент сходства: " + AnalysisResult.PercentOfSimilarity + "%";
    //    return searchResult;
    //}

    #endregion


    #region Properties

    private readonly UserDBContext _userContext;
    private readonly MethodDBContext _methodsContext;
    public List<Method> Counterfeits => _methodsContext.Methods.ToList();
    public Method SelectedCounterfeit { get; set; }

    #endregion


    #region Commands

    private RelayCommand _changePathImage;

    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                //var path = _dialogService.OpenFileDialog(filter: "Pictures (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".jpg");

                //if (path != "")
                //{
                //    DisplayedImagePath = path;
                //    ResultImagePath = "";
                //}
            });
        }
    }

    private RelayCommand _scanImage;

    public RelayCommand ScanImage
    {
        get
        {
            return _scanImage ??= new RelayCommand(_ =>
            {
                //if (DisplayedImagePath is null)
                //{
                //    _messageBoxService.ShowMessage("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                //else
                //{
                //    try
                //    {
                //        //List<MethodPath> counterfeitPaths = new List<MethodPath>();
                //        //if (SelectedCounterfeit is null)
                //        //{
                //        //    counterfeitPaths = _counterfeitsContext.MethodPaths.ToList();
                //        //}
                //        //else
                //        //{
                //        //    counterfeitPaths = _counterfeitsContext.MethodPaths.Include(c => c.Counterfeit).Where(c => c.CounterfeitId == SelectedCounterfeit.Id).ToList();
                //        //}

                //        ////AnalysisResult = _analyzer.RunAnalysis(DisplayedImagePath, counterfeitPaths, PercentOfSimilarity, _userService.User);
                //        //SearchResult = CreateSearchResult(AnalysisResult);

                //        //if (AnalysisResult.ResPath.Path is not null)
                //        //{
                //        //    string pathToBase = Directory.GetCurrentDirectory();
                //        //    string pathToResults = @"..\..\..\resources\resImages\";
                //        //    string combinedPath = Path.Combine(pathToBase, AnalysisResult.ResPath.Path);
                //        //    ResultImagePath = combinedPath;
                //        //}
                //        //_resultContext.Results.Add(AnalysisResult);
                //        //_resultContext.SaveChanges();
                //    }

                //    catch (ArgumentException)
                //    {
                //        _messageBoxService.ShowMessage("Данные в базе фальсификатов были удалены или повреждены.\nПеред запуском анализа устраните проблему.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                //    }
                //}
            });
        }
    }

    private RelayCommand _createFile;

    public RelayCommand CreateFile
    {
        get
        {
            return _createFile ??= new RelayCommand(_ =>
            {
                //if (AnalysisResult is null)
                //{
                //    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                //else
                //{
                //    var filename = "АНАЛИЗ_" + DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
                //    var filePath = _dialogService.SaveFileDialog(filename, ext: ".pdf");
                //    if (!string.IsNullOrEmpty(filePath))
                //    {
                //        //FileSystem.ExportPdf(filePath, AnalysisResult);
                //    }
                //}
            });
        }
    }

    private RelayCommand _changeUser;

    public RelayCommand ChangeUser
    {
        get
        {
            return _changeUser ??= new RelayCommand(_ =>
            {
                _navigationManager.Navigate<LoginControl>(new WindowParameters
                {
                    Height = 300,
                    Width = 350,
                    Title = "Вход в систему",
                    StartupLocation = WindowStartupLocation.CenterScreen,
                });
            });
        }
    }

    private RelayCommand _showInfo;

    public RelayCommand ShowInfo
    {
        get
        {
            return _showInfo ??= new RelayCommand(_ =>
            {
                _messageBoxService.ShowMessage("Данный программный комплекс предназначен для обработки\n" +
                                               "входного изображения среза мясной продукции в задаче\n" +
                                               "обнаружения фальсификата.\n" +
                                               "\n" +
                                               "Вам доступны следующие возможности:\n" +
                                               "   * Анализ изображения.\n" +
                                               "   * Сохранение результата анализа в виде отчёта.\n" +
                                               "\n" +
                                               "Автор:  Ермаков Даниил Игоревич\n" +
                                               "Группа: 494\n" +
                                               "Учебное заведение: СПбГТИ (ТУ)", "Справка о программе",
                                               MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }

    private RelayCommand _exit;

    public RelayCommand Exit
    {
        get
        {
            return _exit ??= new RelayCommand(_ =>
            {
                Application.Current.Shutdown();
            });
        }
    }

    #endregion
}
