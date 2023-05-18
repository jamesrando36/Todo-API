import { useState, useEffect } from "react";
import axios from "axios";
import { TodoItem } from "../interfaces/TodoItem";
import AddTaskForm from "./add-todo-item-component";
import DeleteTaskModal from "./delete-todo-item-component";
import { Container, Typography, Grid, Card, CardContent } from "@mui/material";
import EditTodoItem from "./edit-todo-item-component";

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
      const response = await axios.get("https://localhost:7083/api/TodoItems");
      setTodoItems(response.data);
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
