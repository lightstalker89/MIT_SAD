package com.grillmurrent.gae.java.todo.model;

import java.io.Serializable;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

@Entity
public class History implements Serializable {
	
	/**
	 * 
	 */
	private static final long serialVersionUID = -8755331604856174958L;
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;
	private String author;

	private String summary;
	private String description;
	private String url;
	private String importance;

	public History(String author, String summary, String description, String url, String importance) {
		this.author = author;
		this.summary = summary;
		this.description = description;
		this.url = url;
		this.importance = importance;
	}

	public Long getId() {
		return id;
	}

	public String getAuthor() {
		return author;
	}

	public void setAuthor(String author) {
		this.author = author;
	}

	public String getShortDescription() {
		return summary;
	}

	public void setShortDescription(String shortDescription) {
		this.summary = shortDescription;
	}

	public String getLongDescription() {
		return description;
	}

	public void setLongDescription(String longDescription) {
		this.description = longDescription;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}
	
	public String getImportance() {
		return importance;
	}

	public void setImportance(String importance) {
		this.importance = importance;
	}
}
