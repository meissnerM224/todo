// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteTodo(todoId) {
    $.ajax({
        url: "Home/Delete",
        type: "Post",
        data: {
            id: todoId
        },
        success: () => window.location.reload(),
    });
}
function populateForm(todoId) {
    $.ajax({
        url: "Home/PopulateForm",
        type: "GET",
        data: {
            id: todoId
        },
        dataType: 'json',
        success: function (response) {
            $("#Todo_Name").val(response.name);
            $("#Todo_Id").val(response.id);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action", "/Home/Update");
        }
    });

}