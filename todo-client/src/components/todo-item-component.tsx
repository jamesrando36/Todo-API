import { useState, useEffect } from "react";
import axios from "axios";
import { TodoItem } from "../interfaces/TodoItem";
import AddTaskForm from "./add-todo-item-component";
import DeleteTaskModal from "./delete-todo-item-component";
import { Container, Typography, Grid, Card, CardContent } from "@mui/material";
import EditTodoItem from "./edit-todo-item-component";
import { Box } from "@mui/material";
import zeroStateImage from "../assets/zero-state.png";

function TodoList() {
  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);
  const [completedTasks, setCompletedTasks] = useState<TodoItem[]>([]);

  const handleAddTask = (newTask: TodoItem) => {
    setTodoItems([...todoItems, newTask]);
  };

  const handleDeleteTask = (taskId: number) => {
    const updatedItems = todoItems.filter((item) => item.id !== taskId);
    setTodoItems(updatedItems);
    const updatedCompletedTasks = completedTasks.filter(
      (item) => item.id !== taskId
    );
    setCompletedTasks(updatedCompletedTasks);
  };

  const handleEditTask = (editedTask: TodoItem) => {
    const updatedItems = todoItems.map((item) =>
      item.id === editedTask.id ? editedTask : item
    );
    setTodoItems(updatedItems);
  };

  useEffect(() => {
    async function fetchTodoItems() {
      try {
        const response = await axios.get(
          "https://localhost:7083/api/TodoItems"
        );
        setTodoItems(response.data);
      } catch (error: any) {
        if (error.response && error.response.status === 404) {
          // Handle the 404 "Not Found" response
          setTodoItems([]);
        } else {
          console.error("Error fetching todo items:", error);
        }
      }
    }
    fetchTodoItems();
  }, []);

  useEffect(() => {
    const completedTasksFromTodoItems = todoItems.filter(
      (item) => item.isComplete
    );
    setCompletedTasks(completedTasksFromTodoItems);
  }, [todoItems]);

  return (
    <Container maxWidth="sm" style={{ marginTop: "1em", marginBottom: "1em" }}>
      <Typography variant="h4" align="center" gutterBottom>
        Todo List
      </Typography>

      <div style={{ marginBottom: "5em" }}>
        <AddTaskForm onAddTask={handleAddTask} />
      </div>

      <Grid container spacing={2}>
        {todoItems
          .filter((item) => !item.isComplete) // Filter out completed tasks
          .map((item) => (
            <Grid item key={item.id} xs={12} sm={6}>
              <Card sx={{ height: "100%" }}>
                <CardContent>
                  <Typography variant="h6" component="div">
                    {item.task}
                  </Typography>
                  <Typography color="text.secondary">
                    {item.description}
                  </Typography>
                  <DeleteTaskModal
                    taskId={item.id}
                    onConfirm={handleDeleteTask}
                  />
                  <EditTodoItem
                    todoItem={item}
                    onTodoItemEdit={handleEditTask}
                  />
                </CardContent>
              </Card>
            </Grid>
          ))}
      </Grid>

      <Typography variant="h6" align="center" style={{ marginTop: "2em" }}>
        {completedTasks.length > 0 && "Completed Tasks"}
      </Typography>

      {todoItems.length === 0 && (
        <Box textAlign="center" marginTop="2em">
          <Typography variant="h6" style={{ marginTop: "1em" }}>
            There are currently no tasks created. Please create a task.
          </Typography>
          <img
            src={zeroStateImage}
            alt="No tasks"
            style={{ width: "300px", height: "auto", marginTop: "1em" }}
          />
        </Box>
      )}

      <Grid container spacing={2} style={{ marginTop: "1em" }}>
        {completedTasks.map((item) => (
          <Grid item key={item.id} xs={12} sm={6}>
            <Card sx={{ height: "100%" }}>
              <CardContent>
                <Typography variant="h6" component="div">
                  {item.task}
                </Typography>
                <Typography color="text.secondary">
                  {item.description}
                </Typography>
                <DeleteTaskModal
                  taskId={item.id}
                  onConfirm={handleDeleteTask}
                />
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}

export default TodoList;
