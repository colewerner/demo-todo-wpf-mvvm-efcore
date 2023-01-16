using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;

using Todo.Database.Factory;
using Todo.Database.Models;
using TodoWpf.Model;

namespace TodoWpf.ViewModel;

public partial class TodoViewModel : ObservableRecipient
{
    #region Fields

    private readonly ITodoDbContextFactory todoDbContextFactory;

    private static readonly object syncRoot = new object();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddTodoCommand))]
    private string newTodoItemDescription;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private bool canSaveItems;

    private ObservableCollection<TodoModel> todoModels;

    #endregion

    #region Constructors

    public TodoViewModel(ITodoDbContextFactory todoDbContextFactory)
    {
        this.todoDbContextFactory = todoDbContextFactory;

        this.todoModels = new ObservableCollection<TodoModel>();
        BindingOperations.EnableCollectionSynchronization(this.todoModels,
                                                          syncRoot);

        this.LoadTodoItemsAsyncCommand = new AsyncRelayCommand(cancelableExecute: this.LoadTodoItemsAsync);
        this.SaveCommand = new AsyncRelayCommand(cancelableExecute: this.SaveItems,
                                                 () => this.CanSaveItems);
        this.AddTodoCommand = new AsyncRelayCommand(cancelableExecute: this.AddTodo,
                                                    () => !string.IsNullOrWhiteSpace(value: this.newTodoItemDescription));
        this.DeleteTodoCommand = new AsyncRelayCommand<TodoModel>(cancelableExecute: this.DeleteTodoItem);

        Task.Run(() => this.LoadTodoItemsAsync(new CancellationToken()));
    }

    #endregion

    #region Instance Properties

    public AsyncRelayCommand AddTodoCommand
    {
        get;
        set;
    }

    public AsyncRelayCommand<TodoModel> DeleteTodoCommand
    {
        get;
        set;
    }

    public AsyncRelayCommand LoadTodoItemsAsyncCommand
    {
        get;
        set;
    }

    public AsyncRelayCommand SaveCommand
    {
        get;
        set;
    }

    public ObservableCollection<TodoModel> TodoModels
    {
        get => this.todoModels;
        set =>
            this.SetProperty(field: ref this.todoModels,
                             newValue: value);
    }

    #endregion

    #region Instance Methods

    private async Task AddTodo(CancellationToken cancellationToken)
    {
        using var todoDbContext = this.todoDbContextFactory.CreateContext();

        var newTodo = new Todo.Database.Models.Todo()
                      {
                          Description = this.newTodoItemDescription
                      };

        todoDbContext.Todos.Add(newTodo);

        await todoDbContext.SaveChangesAsync(cancellationToken);

        var newTodoModel = new TodoModel()
                           {
                               Id = newTodo.TodoId,
                               Description = newTodo.Description,
                               IsCompleted = newTodo.IsCompleted
                           };

        newTodoModel.PropertyChanged += this.TodoModelPropertyChanged;

        this.TodoModels.Add(newTodoModel);

        this.NewTodoItemDescription = string.Empty;

        return;
    }

    private async Task DeleteTodoItem(TodoModel? todoItemToDelete,
                                CancellationToken cancellationToken)
    {
        if (todoItemToDelete == null)
            return;

        using var todoDbContext = this.todoDbContextFactory.CreateContext();

        // Delete without querying the db
        todoDbContext.Remove(new Todo.Database.Models.Todo()
                             {
                                 TodoId = todoItemToDelete.Id
                             });

        await todoDbContext.SaveChangesAsync(cancellationToken);

        this.todoModels.Remove(todoItemToDelete);
    }

    private async Task LoadTodoItemsAsync(CancellationToken cancellationToken)
    {
        using var todoDbContext = this.todoDbContextFactory.CreateContext();
        var todos = await todoDbContext.Todos.ToListAsync(cancellationToken: cancellationToken);

        foreach (var todoModel in this.todoModels)
            todoModel.PropertyChanged -= this.TodoModelPropertyChanged;

        var todoModelList = todos.Select(x => new TodoModel
                                              {
                                                  Id = x.TodoId,
                                                  Description = x.Description,
                                                  IsCompleted = x.IsCompleted
                                              })
                                 .ToList();

        todoModelList.ForEach(x => x.PropertyChanged += this.TodoModelPropertyChanged);

        foreach (var todoModel in todoModelList)
        {
            this.todoModels.Add(todoModel);
        }
    }

    private async Task SaveItems(CancellationToken cancellationToken)
    {
        var itemsToSave = this.todoModels.Where(x => x.IsDirty).ToList();

        if (!itemsToSave.Any())
        {
            return;
        }

        using var todoDbContext = this.todoDbContextFactory.CreateContext();

        var dbTodoItemsToUpdate = await todoDbContext.Todos.Where(x => itemsToSave.Select(x => x.Id)
                                                                                  .Contains(x.TodoId))
                                                     .ToListAsync(cancellationToken);

        foreach (var dbTodoItemToUpdate in dbTodoItemsToUpdate)
        {
            var todoModelUpdated = itemsToSave.FirstOrDefault(x => x.Id == dbTodoItemToUpdate.TodoId);

            dbTodoItemToUpdate.IsCompleted = todoModelUpdated.IsCompleted;
            todoModelUpdated.IsDirty = false;
        }

        await todoDbContext.SaveChangesAsync(cancellationToken);

        this.CanSaveItems = false;
    }

    #endregion

    #region Event Handling

    private void TodoModelPropertyChanged(object? sender,
                                          PropertyChangedEventArgs e)
    {
        if (sender == null)
            return;

        var todoModel = sender as TodoModel;
        todoModel.IsDirty = true;
        this.CanSaveItems = true;
    }

    #endregion
}