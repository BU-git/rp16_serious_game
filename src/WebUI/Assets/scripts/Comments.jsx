var CommentBox = React.createClass({
    loadCommentsFromServer: function () {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    },
    handleCommentSubmit: function (comment) {
        var data = new FormData();
        data.append('ParentId', comment.ParentId);
        data.append('Text', comment.Text);
        data.append('Image', comment.Image);
        var xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = function () {
            this.loadCommentsFromServer();
        }.bind(this);
        xhr.send(data);
    },
    getInitialState: function () {
        return { data: this.props.initialData };
    },
    render: function() {
        return (
            <div className="ui comments">
                <h3 className="ui dividing header">Comments</h3>
                <CommentList data={this.state.data} onCommentSubmit={this.handleCommentSubmit} />
                <CommentForm onCommentSubmit={this.handleCommentSubmit} />
            </div>
        );
    }
});

var CommentList = React.createClass({
    render: function () {
    var commentSubmit = this.props.onCommentSubmit;
    var commentNodes = this.props.data.map(function (comment) {
      return (
        <Comment comment={comment} onCommentSubmit={commentSubmit} />
      );
    });
    return (
      <div className="comments">{commentNodes}
      </div>
    );
  }
});

var Comment = React.createClass({
    getInitialState: function () {
        return { reply: false };
    },
    handleClick: function (event) {
        this.setState({ reply: !this.state.reply });
    },
    hideForm: function () {
        this.setState({ reply: false });
    },
    render: function () {
        return (
          <div className="comment">
              <div className="content">
                  <a className="author">{this.props.comment.Author}</a>
                  <div className="metadata">
                    <div className="date">{this.props.comment.Date}</div>
                  </div>
                  <div className="text">
                      {this.props.comment.Text}
                      <div>
                          {this.props.comment.ImagePath ? <a href={this.props.comment.ImagePath}>Attached Image</a> : null}
                      </div>
                  </div>
                  <div className="actions">
                    <a className="reply" onClick={this.handleClick}>Reply</a>
                  </div>
                  { this.state.reply ? <CommentForm onCommentSubmit={this.props.onCommentSubmit} hideForm={this.hideForm} parentId={this.props.comment.Id} /> : null }
                  <CommentList data={this.props.comment.Children} onCommentSubmit={this.props.onCommentSubmit} />
              </div>
          </div>
      );
    }
});

var CommentForm = React.createClass({
    handleSubmit: function (e) {
        e.preventDefault();
        var parentId = this.props.parentId;
        var text = this.refs.text.value.trim();
        var image = ReactDOM.findDOMNode(this.refs.image).files[0];
        if (!text && !image) {
            return;
        }
        this.props.onCommentSubmit({ ParentId: parentId, Text: text, Image: image });
        this.refs.text.value = '';
        this.refs.image.value = '';
        this.props.hideForm();
        return;
    },
    render: function() {
        return (
          <form  className="ui reply form" onSubmit={this.handleSubmit} enctype = "multipart/form-data">
            <div className="field">
              <input type="text" placeholder="Say something..." ref="text" />
            </div>
              <div className="field">
              <input type="file" ref="image" />
              </div>
            <button className="ui primary submit labeled icon button" type="submit">
              <i className="icon edit"></i> Add Reply
            </button>
          </form>
      );
    }
});

