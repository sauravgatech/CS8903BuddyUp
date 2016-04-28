@model $rootnamespace$.Models.UserViewModel

@{
    ViewBag.Title = "Delete";
}

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<fieldset>
    <legend>UserProfile</legend>

    <div class="display-label">UserName</div>
    <div class="display-field">@Model.UserName</div>
    
    <div class="display-label">Email</div>
    <div class="display-field">@Model.Email</div>
    
    <div class="display-label">Role</div>
    <div class="display-field">@Model.Role</div>

</fieldset>
@using (Html.BeginForm()) {
    <p>
        <input type="submit" value="Delete" /> |
        @Html.ActionLink("Back to List", "Index")
    </p>
}


