exports.handler = (event, context, callback) => {
    
    var responseBody = [
        {
            key: 1, 
            name: "Nefils",
            description: "Nefils practice to carnival",
            avatar: "data:image/jpeg;base64,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"
        },
        {
            key: 2, 
            name: "Teck Reunion",
            description: "Practice in the itaipava's sound",
            avatar: "https://www.grupopetropolis.com.br/grpptrpls/wp-content/uploads/itaipava.png"
        },
        {
            key: 3, 
            name: "System of Systems",
            description: "Songs of Rural's practice",
            avatar: "https://d1i0fc51bv4e6i.cloudfront.net/noticias/wp-content/uploads/2015/10/14110916/ufrrj.png"
        }
    ];

    var response = {
        "statusCode": 200,
        "body": JSON.stringify(responseBody),
        "isBase64Encoded": false
    };
    callback(null, response);
};