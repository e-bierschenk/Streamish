import React, { useEffect, useState } from "react";
import Video from './Video';
import { getUsersVideos } from "../modules/videoManager";
import { useParams } from "react-router-dom";

const VideoList = () => {
    const [userVideos, setUserVideos] = useState([]);
    const { id } = useParams();

    const getVideos = () => {
        getUsersVideos(id).then(videos => setUserVideos(videos));
    };

    useEffect(() => {
        getVideos();
    }, []);

    if (!userVideos) {
        return null;
      }

    return (
        <>
            <div className="container">
                <div className="row justify-content-center">
                    {userVideos.videos?.map((video) => (
                        <Video video={video} userName={userVideos.name} key={`video-${video.id}`} />
                    ))}
                </div>
            </div>
        </>
    );
};

export default VideoList;