@model $rootnamespace$.Models.UserViewModel

@* This partial view defines form fields that will appear when creating and editing entities *@

<div class="editor-label">
    @Html.LabelFor(model => model.UserName)
</div>
<div class="editor-field">
    @Html.EditorFor(model => model.UserName)
    @Html.ValidationMessageFor(model => model.UserName)
</div>

<div class="editor-label">
    @Html.LabelFor(model => model.Email)
</div>
<div class="editor-field">
    @Html.EditorFor(model => model.Email)
    @Html.ValidationMessageFor(model => model.Email)
</div>

<div class="editor-label">
    @Html.LabelFor(model => model.SelectedRole)
</div>
<div class="editor-field">
    @Html.DropDownListFor(model => model.SelectedRole, new SelectList(ViewBag.AvailableRoles, Model.SelectedRole),"Select")
    @Html.ValidationMessageFor(model => model.Role)
</div>

