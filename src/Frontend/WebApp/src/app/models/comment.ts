export interface IComment {
    id: number
    userId: number
    postId: number
    parentCommentId: number
    content: string
    creationTimeStamp: string
    comments: IComment[]
}