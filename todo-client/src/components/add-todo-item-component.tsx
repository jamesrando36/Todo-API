import React, { useState } from "react";
import axios from "axios";
import { Button, TextField, Typography } from "@material-ui/core";

function AddTaskForm({ onAddTask }: { onAddTask: any }) {
  const [newTask, setNewTask] = useState({
    task: "",
    description: "",
  });

  const [error, setError] = useState("");

  const handleTaskChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewTask({ ...newTask, task: e.target.value });
  };

  const handleDescriptionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewTask({ ...newTask, description: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (newTask.task === "") {
      setError("Task is required");
      return;
    }

    try {
      const response = await axios.post(
        "https://localhost:7083/api/TodoItems",
        newTask
      );
      onAddTask(response.data);
      setNewTask({ task: "", description: "" });
      setError("");
    } catch (error) {
      console.error("Error adding task:", error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <Typography variant="h6" color="error" align="center">
        {error}
      </Typography>
      <TextField
        fullWidth
        variant="outlined"
        label="New task"
        value={newTask.task}
        onChange={handleTaskChange}
        margin="normal"
        error={error !== ""}
      />
      <TextField
        fullWidth
        variant="outlined"
        label="Description"
        value={newTask.description}
        onChange={handleDescriptionChange}
        margin="normal"
      />
      <Button variant="contained" color="primary" type="submit">
        Add
      </Button>
    </form>
  );
}

export default AddTaskForm;
