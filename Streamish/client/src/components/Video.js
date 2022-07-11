import React from "react";
import { Card, CardBody } from "reactstrap";
import Comment from './Comment';
import { Link } from "react-router-dom";

const Video = ({ video, userName }) => {
    return (
        <Card >
            {userName ? 
            <p className="text-left px-2">Posted by: {userName} </p>
            : 
            <p className="text-left px-2">Posted by: <Link to={`/users/${video.userProfile.id}`}>{video.userProfile?.name}</Link></p>}
            <CardBody>
                <iframe className="video"
                    src={video.url}
                    title="YouTube video player"
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen />

                <p>
                    <Link to={`/videos/${video.id}`}>
                        <strong>{video.title}</strong>
                    </Link>
                </p>
                <p>{video.description}</p>
            </CardBody>
            {video.comments?.map(comment => (
                <div className="container">

                    <Comment comment={comment} key={`comment-${comment.id}`} />

                </div>
            ))}
        </Card>
    );
};

export default Video;