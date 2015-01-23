package com.grillmurrent.gae.java.todo.dao;

import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Query;

import com.grillmurrent.gae.java.todo.model.History;
import com.grillmurrent.gae.java.todo.model.Todo;

public enum Dao {
	INSTANCE;

	public List<Todo> listTodos() {
		EntityManager em = EMFService.get().createEntityManager();
		// read the existing entries
		Query q = em.createQuery("select m from Todo m");
		List<Todo> todos = q.getResultList();
		return todos;
	}

	public void add(String userId, String summary, String description,
			String url, String importance) {
		synchronized (this) {
			EntityManager em = EMFService.get().createEntityManager();
			Todo todo = new Todo(userId, summary, description, url, importance);
			em.persist(todo);
			em.close();
		}
	}

	public List<Todo> getTodos(String userId) {
		EntityManager em = EMFService.get().createEntityManager();
		Query q = em
				.createQuery("select t from com.grillmurrent.gae.java.todo.model.Todo t where t.author = :userId");
		q.setParameter("userId", userId);
		List<Todo> todos = q.getResultList();
		return todos;
	}
	
	public void addHistory(String userId, String summary, String description,
			String url, String importance) {
		synchronized (this) {
			EntityManager em = EMFService.get().createEntityManager();
			History todo = new History(userId, summary, description, url, importance);
			em.persist(todo);
			em.close();
		}
	}

	public List<History> getHistory(String userId) {
		EntityManager em = EMFService.get().createEntityManager();
		Query q = em
				.createQuery("select t from com.grillmurrent.gae.java.todo.model.History t where t.author = :userId");
		q.setParameter("userId", userId);
		List<History> history = q.getResultList();
		return history;
	}

	public Todo getTodo(long id) {
		EntityManager em = EMFService.get().createEntityManager();
		try {
			return em.find(Todo.class, id);
		} finally {
			em.close();
		}
	}
	
	public void remove(long id) {
		EntityManager em = EMFService.get().createEntityManager();
		try {
			Todo todo = em.find(Todo.class, id);
			em.remove(todo);
		} finally {
			em.close();
		}
	}
}
