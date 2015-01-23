package com.grillmurrent.gae.java.todo;

import java.io.IOException;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.grillmurrent.gae.java.todo.dao.Dao;
import com.grillmurrent.gae.java.todo.model.Todo;

public class ServletRemoveTodo extends HttpServlet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 8364165381461238900L;

	public void doGet(HttpServletRequest req, HttpServletResponse resp)
			throws IOException {
		String id = req.getParameter("id");
		Todo todo = Dao.INSTANCE.getTodo(Long.parseLong(id));
		Dao.INSTANCE.remove(Long.parseLong(id));
		Dao.INSTANCE.addHistory(
				todo.getAuthor(),
				todo.getShortDescription(),
				todo.getLongDescription(),
				todo.getUrl(),
				todo.getImportance());
		resp.sendRedirect("/TodoApplication.jsp");
	}
}
