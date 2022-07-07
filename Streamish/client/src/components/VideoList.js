import React, { useEffect, useState } from "react";
import Video from './Video';
import { getAllVideos, searchVideos } from "../modules/videoManager";

const VideoList = () => {
    const [videos, setVideos] = useState([]);

    const getVideos = () => {
        getAllVideos().then(videos => setVideos(videos));
    };

    const handleFieldChange = (event) => {
        searchVideos(event.target.value)
            .then(videos => setVideos(videos))
    }

    useEffect(() => {
        getVideos();
    }, []);

    return (
        <>
            <input type="text" onChange={handleFieldChange} />
            <div className="container">
                <div className="row justify-content-center">
                    {videos.map((video) => (
                        <Video video={video} key={`video-${video.id}`} />
                    ))}
                </div>
            </div>
        </>
    );
};

export default VideoList;