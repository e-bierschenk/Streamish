import React from "react";
import { Routes, Route } from "react-router-dom";
import VideoList from "./VideoList";
import VideoForm from "./VideoForm";
import UserVideos from "./UserVideos";
import VideoDetails from "./VideoDetails";

const ApplicationViews = () => {
    return (
        <Routes>
            <Route path="/" >
                <Route index element={<VideoList />} />
                <Route path="videos">
                    <Route index element={<VideoList />} />
                    <Route path="add" element={<VideoForm />} />
                    <Route path=":id" element={<VideoDetails />} />
                </Route>
                <Route path="users">
                    <Route index element={<p>coming soon...</p>} />
                    <Route path=":id" element={<UserVideos /> } />
                </Route>
            </Route>
            <Route path="*" element={<p>Whoops, nothing here...</p>} />
        </Routes>
    );
};

export default ApplicationViews;