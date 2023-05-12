import React, { useState } from "react";
import { IconButton, Modal, Typography, Button } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";

function DeleteTaskModal({
  taskId,
  onConfirm,
}: {
  taskId: any;
  onConfirm: any;
}) {
  const [open, setOpen] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleConfirm = () => {
    onConfirm(taskId);
    handleClose();
  };

  return (
    <>
      <IconButton onClick={handleOpen}>
        <DeleteIcon />
      </IconButton>
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
            Are you sure you want to delete this task?
          </Typography>
          <Button variant="contained" color="primary" onClick={handleConfirm}>
            Delete
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

export default DeleteTaskModal;
