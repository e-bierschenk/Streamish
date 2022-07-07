import React from "react";
import { Card, CardBody } from "reactstrap";
import Comment from './Comment';

const Video = ({ video }) => {
    return (
        <Card >
            <p className="text-left px-2">Posted by: {video.userProfile.name}</p>
            <CardBody>
                <iframe className="video"
                    src={video.url}
                    title="YouTube video player"
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen />

                <p>
                    <strong>{video.title}</strong>
                </p>
                <p>{video.description}</p>
            </CardBody>
            {video.comments?.map(comment => (
                <div className="container">
                    
                        <Comment comment={comment} key={`comment-${comment.id}`}/>
                
                </div>
            ))}
        </Card>
    );
};

export default Video;