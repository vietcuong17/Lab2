﻿using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace MangaReader.MangaList;

public partial class View : Window, IView
{

    private readonly Presenter? presenter;
    private readonly List<ItemControl> itemControls = new();
    
    public View()
    {
        InitializeComponent();
    }

    public View(Presenter presenter) : this()
    {
        this.presenter = presenter;
        this.ErrorPanel.RetryButton.Click += (sender, args) => this.presenter?.Load();
    }

    public void SetLoadingVisible(bool value)
    {
        this.LoadingProgressBar.IsVisible = value;
    }

    public void SetErrorPanelVisible(bool value)
    {
        this.ErrorPanel.IsVisible = value;
    }
    // ---------- 2 ---------
    public void SetMainContentVisible(bool value)
    {
        this.MainContent.IsVisible = value;
    }

    public void SetTotalMangaNumber(string text)
    {
        this.TotalMangaNumberLabel.Content = text;
    }

    public void SetCurrentPageButtonContent(string content)
    {
        this.CurrentPageButton.Content = content;
    }

    public void SetCurrentPageButtonEnabled(bool value)
    {
        this.CurrentPageButton.IsEnabled = value;
    }

    public void SetNumericUpDownMaximum(int value)
    {
        this.NumericUpDown.Maximum = value;
    }

    public void SetNumericUpDownValue(int value)
    {
        this.NumericUpDown.Value = value;
    }

    public int GetNumericUpDownValue()
    {
        return (int)this.NumericUpDown.Value!;
    }

    public void SetListBoxContent(IEnumerable<Item> items)
    {
        itemControls.Clear();
        this.MangaListBox.Items.Clear();
        foreach (var item in items)
        {
            var itemControl = new ItemControl();
            itemControl.TitleTextBlock.Text = item.Title;
            itemControl.ChapterNumberTextBlock.Text = item.ChapterNumber;
            ToolTip.SetTip(itemControl.CoverBorder, item.ToolTip);
            itemControls.Add(itemControl);
            this.MangaListBox.Items.Add(new ListBoxItem { Content = itemControl});
        }
    }

    public void SetCover(int index, byte[]? bytes)
    {
        var errorCoverBackground = Brushes.DeepPink;
        var itemControl = itemControls[index];
        if (bytes == null)
        {
            itemControl.CoverBorder.Background = errorCoverBackground;
            return;
        }

        using var ms = new MemoryStream(bytes);
        try
        {
            itemControl.CoverImage.Source = new Bitmap(ms);
        }
        catch (Exception)
        {
            itemControl.CoverBorder.Background = errorCoverBackground;
        }
    }

    public void SetFirstButtonAndPrevButtonEnabled(bool value)
    {
        this.FirstButton.IsEnabled = value;
        this.PrevButton.IsEnabled = value;
    }

    public void SetLastButtonAndNextButtonEnabled(bool value)
    {
        this.LastButton.IsEnabled = value;
        this.NextButton.IsEnabled = value;
    }

    public void HideFlyout()
    {
        this.CurrentPageButton.Flyout?.Hide();
    }

    public void SetErrorMessage(string text)
    {
        this.ErrorPanel.MessageTextBlock.Text = text;
    }

    public string? GetFilterText()
    {
        return this.MyTextBox.Text;
    }


    private void MyListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        Console.WriteLine("selected: " + this.MangaListBox.SelectedIndex);
    }

    private void MyListBox_OnkeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Console.WriteLine("select: " + this.MangaListBox.SelectedIndex);
        }
    }
    // -----------------------------------------------------------------------
    private void RefreshButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.Load();
    }

    private void PrevButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.GoPrevPage();
    }

    private void NextButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.GoNextPage();
    }

    private void FirstButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.GoFirstPage();
    }

    private void LastButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.GoLastPage();
    }

    private void GoButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.GoSpecificPage();
    }

    private void NumericUpDown_OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        presenter?.GoSpecificPage();
    }

    private void MyClearButtom_OnClick(object? sender, RoutedEventArgs e)
    {
        if (this.MyTextBox.Text == null) return;
        else
        {
            this.MyTextBox.Text = "";
        }
    }

    private void MyApplyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        presenter?.ApplyFilter();
    }

    private void OnkeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            presenter?.ApplyFilter();
        }
    }
    
}