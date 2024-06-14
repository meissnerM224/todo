using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Controllers;

public class HomeController : Controller
{
    private readonly string dbName = "Data Source=db.sqlite";
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var todoListModel = GetAllTodos();
        return View(todoListModel);
    }

    [HttpPost]

    public JsonResult Delete(int Id)
    {
        using SqliteConnection con = new(dbName);
        using var tableCmd = con.CreateCommand();
        con.Open();
        tableCmd.CommandText = $"DELETE from todo WHERE Id = '{Id}'";
        tableCmd.ExecuteNonQuery();
        return Json(new { });
    }

    [HttpGet]

    public JsonResult PopulateForm(int Id)
    {
        var todo = GetById(Id);

        return Json(todo);
    }
    internal TodoItem? GetById(int Id)
    {
        TodoItem todo = new()
        {
            Id = 0,
            Name = ""
        };
        using SqliteConnection con = new(dbName);
        using var tableCmd = con.CreateCommand();
        con.Open();
        tableCmd.CommandText = $"SELECT * FROM todo Where Id = '{Id}'";
        using var reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();

            todo.Id = reader.GetInt32(0);
            todo.Name = reader.GetString(1);
        }
        return todo;
    }

    public RedirectResult Update(TodoItem? todo)
    {
        if (todo == null)
        {
            return Redirect("https://localhost:7092");
        }

        using SqliteConnection con = new(dbName);
        using var tableCmd = con.CreateCommand();
        con.Open();
        tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Redirect("https://localhost:7092");
    }

    internal TodoViewModel GetAllTodos()
    {
        List<TodoItem> todoList = [];
        {
            using SqliteConnection con = new(dbName);

            using var tableCmd = con.CreateCommand();

            con.Open();
            tableCmd.CommandText = "SELECT * FROM todo";

            using var reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    todoList.Add(
                        new TodoItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        });
                }
            }
            else
            {

                return new TodoViewModel
                {
                    TodoList = todoList
                };
            }

            return new TodoViewModel
            {
                TodoList = todoList
            };
        }
    }
    public RedirectResult Insert(TodoItem todo)
    {
        using SqliteConnection con = new(dbName);
        using var tableCmd = con.CreateCommand();
        con.Open();
        tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Redirect("https://localhost:7092");

    }

    // public IActionResult Privacy()
    // {
    //     return View();
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
