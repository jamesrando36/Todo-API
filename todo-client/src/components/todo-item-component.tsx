import React, { useState, useEffect } from "react";
import axios from "axios";
import {
  Container,
  List,
  ListItem,
  ListItemText,
  Typography,
  makeStyles,
} from "@material-ui/core";
import { TodoItem } from "../interfaces/TodoItem";
import AddTaskForm from "./add-todo-item-component";
import DeleteTaskModal from "./delete-todo-item-component";

const useStyles = makeStyles((theme) => ({
  container: {
    marginTop: theme.spacing(4),
    marginBottom: theme.spacing(4),
  },
  title: {
    marginBottom: theme.spacing(2),
  },
}));

function TodoList() {
  const classes = useStyles();

  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);

  const handleAddTask = (newTask: TodoItem) => {
    setTodoItems([...todoItems, newTask]);
  };

  const handleDeleteTask = (taskId: number) => {
    const updatedItems = todoItems.filter((item) => item.id !== taskId);
    setTodoItems(updatedItems);
  };

  useEffect(() => {
    async function fetchTodoItems() {
      const response = await axios.get("https://localhost:7083/api/TodoItems");
      setTodoItems(response.data);
    }
    fetchTodoItems();
  }, []);

  return (
    <Container maxWidth="sm" className={classes.container}>
      <Typography variant="h4" align="center" gutterBottom className={classes.title}>
        Todo List
      </Typography>

      <AddTaskForm onAddTask={handleAddTask} />

      <List>
        {todoItems.map((item) => (
          <ListItem key={item.id}>
            <ListItemText primary={item.task} secondary={item.description} />
            <DeleteTaskModal taskId={item.id} onConfirm={handleDeleteTask} />
          </ListItem>
        ))}
      </List>
    </Container>
  );
}

export default TodoList;
