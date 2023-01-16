using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoWpf.Model;

public partial class TodoModel : ObservableObject, IStateTracking
{
    #region Instance Properties

    [ObservableProperty]
    string description;

    [ObservableProperty]
    int id;

    [ObservableProperty]
    bool isCompleted;

    #endregion

    public bool IsDirty
    {
        get;
        set;
    }
}