export interface IComment {
    id: number
    userId: number
    postId: number
    content: string
    creationTimeStamp: string
    comments: IComment[]
}