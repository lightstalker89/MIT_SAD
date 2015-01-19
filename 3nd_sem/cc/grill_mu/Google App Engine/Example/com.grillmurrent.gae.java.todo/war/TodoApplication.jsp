<%@ page contentType="text/html;charset=UTF-8" language="java"%>
<%@ page import="java.util.List"%>
<%@ page import="com.google.appengine.api.users.User"%>
<%@ page import="com.google.appengine.api.users.UserService"%>
<%@ page import="com.google.appengine.api.users.UserServiceFactory"%>
<%@ page import="com.grillmurrent.gae.java.todo.model.Todo"%>
<%@ page import="com.grillmurrent.gae.java.todo.dao.Dao"%>

<!DOCTYPE html>


<%@page import="java.util.ArrayList"%>

<html>
<head>
<title>Todos</title>
<link rel="stylesheet" type="text/css" href="css/main.css" />
<script type="text/javascript" src="script/jquery-1.11.2.min.js" ></script>
<script type="text/javascript" src="script/jquery.tablesorter.min.js" ></script>
<script type="text/javascript">
	$(document).ready(function() {
		if($("#keywords > tbody > tr").length > 0){
			$('#keywords').tablesorter();
		}
	});
</script>
<meta charset="utf-8">
</head>
<body>
	<%
		Dao dao = Dao.INSTANCE;

		UserService userService = UserServiceFactory.getUserService();
		User user = userService.getCurrentUser();

		String url = userService.createLoginURL(request.getRequestURI());
		String urlLinktext = "Login";
		List<Todo> todos = new ArrayList<Todo>();

		if (user != null) {
			url = userService.createLogoutURL(request.getRequestURI());
			urlLinktext = "Logout";
			todos = dao.getTodos(user.getUserId());
		}
	%>
	<div style="width: 100%;">
		<div class="line"></div>
		<div class="topLine">
			<div style="float: left;">
				<img src="images/todo.png" />
			</div>
			<div style="float: left;" class="headline">Todos</div>
			<div style="float: right;">
				<a href="<%=url%>"><%=urlLinktext%></a>
				<%=(user == null ? "" : user.getNickname())%></div>
		</div>
	</div>

	<div style="clear: both;" />
	You have a total number of
	<%=todos.size()%>
	Todos.

	<table id="keywords">
		<thead>
			<tr>
				<th>Short description</th>
				<th>Long Description</th>
				<th>URL</th>
				<th class="sortable headerSortDown"><span>Importance</span></th>
				<th>Done</th>
			</tr>
		</thead>
		<%
			for (Todo todo : todos) {
		%>
		<tr>
			<td><%=todo.getShortDescription()%></td>
			<td><%=todo.getLongDescription()%></td>
			<td><%=todo.getUrl()%></td>
			<td><%=todo.getImportance()%></td>
			<td><a class="done" href="/done?id=<%=todo.getId()%>">Done</a></td>
		</tr>
		<%
			}
		%>
	</table>


	<hr />

	<div class="main">

		<div class="headline">New todo</div>

		<%
			if (user != null) {
		%>

		<form action="/new" method="post" accept-charset="utf-8">
			<table id="content">
				<tr>
					<td><label for="summary">Summary</label></td>
					<td><input type="text" name="summary" id="summary" size="65" /></td>
				</tr>
				<tr>
					<td valign="description"><label for="description">Description</label></td>
					<td><textarea rows="4" cols="50" name="description"
							id="description"></textarea></td>
				</tr>
				<tr>
					<td valign="top"><label for="url">URL</label></td>
					<td><input type="url" name="url" id="url" size="65" /></td>
				</tr>
				<tr>
					<td><label for="importance">Importance</label></td>
					<td><input type="number" name="importance" id="importance"
						min="1" max="10" /></td>
				</tr>
				<tr>
					<td colspan="2" align="right"><input type="submit"
						value="Create" /></td>
				</tr>
			</table>
		</form>

		<%
			} else {
		%>

		Please login with your Google account

		<%
			}
		%>
	</div>
</body>
</html>
