import React from "react";
import ReactDOM from "react-dom/client";  // Update the import
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import "./index.css";

// Change ReactDOM.render() to ReactDOM.createRoot()
const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <BrowserRouter>
    <App />
  </BrowserRouter>
);
