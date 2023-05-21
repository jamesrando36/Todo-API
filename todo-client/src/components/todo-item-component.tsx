import { useState, useEffect } from "react";
import axios from "axios";
import { TodoItem } from "../interfaces/TodoItem";
import AddTaskForm from "./add-todo-item-component";
import DeleteTaskModal from "./delete-todo-item-component";
import DeleteAllTasksComponent from "./delete-all-todo-item-component";
import {
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  CircularProgress,
  Box,
} from "@mui/material";
import EditTodoItem from "./edit-todo-item-component";
import zeroStateImage from "../assets/zero-state.png";

function TodoList() {
  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);
  const [completedTasks, setCompletedTasks] = useState<TodoItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);

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

  const handleDeleteAllTasks = () => {
    setTodoItems([]);
    setCompletedTasks([]);
  };

  useEffect(() => {
    async function fetchTodoItems() {
      try {
        const response = await axios.get(
          "https://localhost:7083/api/TodoItems"
        );

        setTodoItems(response.data);
        setIsLoading(false);
      } catch (error: any) {
        if (error.response && error.response.status === 404) {
          // Handle the 404 "Not Found" response
          setTodoItems([]);
        } else {
          console.error("Error fetching todo items:", error);
        }
        setIsLoading(false);
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
    <Container
      maxWidth="sm"
      style={{
        marginTop: "1em",
        marginBottom: "1em",
        backgroundColor: "#f5f5f5",
      }}
    >
      <Typography variant="h4" align="center" gutterBottom>
        Todo List
      </Typography>

      <div style={{ marginBottom: "2em" }}>
        <AddTaskForm onAddTask={handleAddTask} />
        {todoItems.length >= 2 && (
          <DeleteAllTasksComponent onConfirm={handleDeleteAllTasks} />
        )}
      </div>

      {isLoading ? (
        <Box textAlign="center" marginTop="2em">
          <CircularProgress />
        </Box>
      ) : (
        <>
          <Grid container spacing={2}>
            {todoItems
              .filter((item) => !item.isComplete)
              .map((item) => (
                <Grid item key={item.id} xs={12} sm={6}>
                  <Card sx={{ height: "100%", backgroundColor: "#ffffff" }}>
                    <CardContent>
                      <Typography variant="h6" component="div">
                        {item.task}
                      </Typography>
                      <Typography color="text.secondary">
                        {item.description}
                      </Typography>
                      <Typography color="text.secondary">
                        {item.formattedTaskTimestamp}
                      </Typography>
                      <Box
                        sx={{
                          display: "flex",
                          justifyContent: "space-between",
                          marginTop: "1em",
                        }}
                      >
                        <DeleteTaskModal
                          taskId={item.id}
                          onConfirm={handleDeleteTask}
                        />
                        <EditTodoItem
                          todoItem={item}
                          onTodoItemEdit={handleEditTask}
                        />
                      </Box>
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
                <Card sx={{ height: "100%", backgroundColor: "#ffffff" }}>
                  <CardContent>
                    <Typography variant="h6" component="div">
                      {item.task}
                    </Typography>
                    <Typography color="text.secondary">
                      {item.description}
                    </Typography>
                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "flex-end",
                        marginTop: "1em",
                      }}
                    >
                      <DeleteTaskModal
                        taskId={item.id}
                        onConfirm={handleDeleteTask}
                      />
                    </Box>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        </>
      )}
    </Container>
  );
}

export default TodoList;
