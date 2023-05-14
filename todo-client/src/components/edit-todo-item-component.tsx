import { useState } from "react";
import { TodoItem } from "../interfaces/TodoItem";
import { IconButton } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField } from "@mui/material";

interface EditTodoItemProps {
  todoItem: TodoItem;
  onTodoItemEdit: (editedTask: TodoItem) => void;
}

function EditTodoItem({ todoItem, onTodoItemEdit }: EditTodoItemProps) {
  const [open, setOpen] = useState(false);
  const [editedTask, setEditedTask] = useState<TodoItem>({
    id: todoItem.id,
    task: todoItem.task,
    description: todoItem.description,
    isComplete: todoItem.isComplete,
    date: todoItem.date
  });

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleTaskChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEditedTask({ ...editedTask, task: e.target.value });
  };

  const handleDescriptionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEditedTask({ ...editedTask, description: e.target.value });
  };

  const handleSave = () => {
    onTodoItemEdit(editedTask);
    handleClose();
  };

  return (
    <>
      <IconButton aria-label="edit" onClick={handleOpen} size="small" color="primary">
        <EditIcon />
      </IconButton>
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Edit Todo Item</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            variant="outlined"
            label="Task"
            value={editedTask.task}
            onChange={handleTaskChange}
            margin="normal"
          />
          <TextField
            fullWidth
            variant="outlined"
            label="Description"
            value={editedTask.description}
            onChange={handleDescriptionChange}
            margin="normal"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button onClick={handleSave} color="primary">
            Save
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}

export default EditTodoItem;
