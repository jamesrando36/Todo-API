import { useState } from "react";
import axios from "axios";
import { Button, Modal, Typography } from "@mui/material";

function DeleteAllTasksModal({ onConfirm }: { onConfirm: () => void }) {
  const [open, setOpen] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleConfirm = async () => {
    try {
      await axios.delete("https://localhost:7083/api/TodoItems");
      onConfirm();
    } catch (error) {
      console.error("Error deleting all tasks:", error);
    } finally {
      handleClose();
    }
  };

  return (
    <>
      <Button
        variant="contained"
        color="error"
        style={{ marginTop: "0.5em" }}
        onClick={handleOpen}
      >
        Delete All
      </Button>
      <Modal open={open} onClose={handleClose}>
        <div
          style={{
            margin: "auto",
            width: 300,
            padding: 16,
            backgroundColor: "#fff",
            borderRadius: 4,
          }}
        >
          <Typography variant="h6" gutterBottom>
            Confirm Deletion
          </Typography>
          <Typography variant="body1" gutterBottom>
            Are you sure you want to delete all tasks?
          </Typography>
          <Button variant="contained" color="primary" onClick={handleConfirm}>
            Delete All
          </Button>
          <Button
            variant="outlined"
            color="primary"
            onClick={handleClose}
            style={{ marginLeft: 16 }}
          >
            Cancel
          </Button>
        </div>
      </Modal>
    </>
  );
}

export default DeleteAllTasksModal;
