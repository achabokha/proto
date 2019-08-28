import { TreeNode } from "../comments.service";

export const commentTreeMock: TreeNode[] = [
    {
        name: "Segin",
        id: "1",
        parentId: null,
        content: "This highlights are great. 🍫 <br> I think I might do more of it. 🚗 ✈ Test.",
        likes: 10,
        dislikes: 0,
        datetime: "2019-01-01T21:49:21.024",
        children: [
            {
                name: "John",
                id: "3",
                parentId: "1",
                content: "You might want to consider to widen your options.😋",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: []
            },
            {
                name: "Peter",
                id: "4",
                parentId: "1",
                content: "You might want to consider to widen your options.",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: []
            }
        ]
    },
    {
        name: "Peter",
        id: "2",
        parentId: null,
        content: "This highlights are great. I think I might do more of it. 😁",
        likes: 10,
        dislikes: 0,
        datetime: "2019-01-01T21:49:21.024",
        children: []
    },
    {
        name: "John",
        id: "5",
        parentId: null,
        content: "This highlights are great. I think I might do more of it. 🤣",
        likes: 10,
        dislikes: 0,
        datetime: "2019-01-01T21:49:21.024",
        children: [
            {
                name: "John",
                id: "12",
                parentId: "5",
                content: "You might want to consider to widen your options.😋",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: []
            },
            {
                name: "Peter",
                id: "13",
                parentId: "5",
                content: "You might want to consider to widen your options.",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: []
            }
        ]
    },
    {
        name: "Karen",
        id: "6",
        parentId: null,
        content: "This highlights are great. I think I might do more of it. ",
        likes: 10,
        dislikes: 0,
        datetime: "2019-01-01T21:49:21.024",
        children: [
            {
                name: "John",
                id: "7",
                parentId: null,
                content: "This highlights are great. I think I might do more of it. ",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: []
            },
            {
                name: "John",
                id: "8",
                parentId: null,
                content: "This highlights are great. I think I might do more of it. ",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: []
            },
            {
                name: "John",
                id: "9",
                parentId: null,
                content: "This highlights are great. I think I might do more of it. ",
                likes: 10,
                dislikes: 0,
                datetime: "2019-01-01T21:49:21.024",
                children: [            {
                    name: "Peter",
                    id: "10",
                    parentId: null,
                    content: "This highlights are great. I think I might do more of it. ",
                    likes: 10,
                    dislikes: 0,
                    datetime: "2019-01-01T21:49:21.024",
                    children: []
                },
                {
                    name: "John",
                    id: "11",
                    parentId: null,
                    content: "This highlights are great. I think I might do more of it. ",
                    likes: 10,
                    dislikes: 0,
                    datetime: "2019-01-01T21:49:21.024",
                    children: []
                }]
            },

        ]
    }
];
