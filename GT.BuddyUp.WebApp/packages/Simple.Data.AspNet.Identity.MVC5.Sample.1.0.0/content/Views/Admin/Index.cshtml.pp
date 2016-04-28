@model IEnumerable<$rootnamespace$.Models.UserViewModel>

@{
    ViewBag.Title = "Index";
}

<h2>Administration</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table>
    <tr>
        <th></th>
        <th>
            UserName
        </th>
        <th>
            Email
        </th>
        <th>
            Confirmed
        </th>
        <th>
            Locked
        </th>
    </tr>

    @foreach (var item in Model)
    {
        <tr>
            <td>
                @if (item.Locked) {
                    @Html.ActionLink("Unlock", "Unlock", new { id = item.Id } )
                }
                else {
                    <span style="color: lightgrey">Unlock</span>
                }
                |
                @Html.ActionLink("Delete", "Delete", new { id = item.Id })
            </td>
            <td>
                @item.UserName
            </td>
            <td>
                @item.Email
            </td>
            <td>
                @item.Confirmed
            </td>
            <td>
                @item.Locked
            </td>
        </tr>
    }

</table>
