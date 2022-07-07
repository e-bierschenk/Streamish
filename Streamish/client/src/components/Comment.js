import React from "react";
import { Card, CardBody, CardTitle, CardText } from "reactstrap";


const Comment = ({ comment }) => {

    return (
        <div>
            <Card >
                    <CardText>
                    {comment.userProfileId}: {comment.message}
                    </CardText>
            </Card>
        </div>
    )
}

export default Comment;