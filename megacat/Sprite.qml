import QtQuick 1.1

Item {
    id: sprite

    property string animationName//: "vxod"
    property int frames//: 6
    property int currentFrame: 0
    property alias running: spriteTimer.running
    property bool repeat: true
    property alias interval: spriteTimer.interval

    onRepeatChanged: {
        if (sprite.repeat)
            spriteTimer.running = true;
    }

    width: 512
    height: 512

    onAnimationNameChanged: sprite.currentFrame = 0

    Image {
        id: spriteFrame
        source: "images/" + sprite.animationName + "/" + sprite.animationName + sprite.currentFrame + ".png"

        anchors.fill: parent
    }

    Timer {
        id: spriteTimer
        interval: 100
        repeat: true
        running: true
        onTriggered: {
            if (sprite.currentFrame < sprite.frames - 1) {
                ++sprite.currentFrame;
            }
            else {
                if (sprite.repeat)
                    sprite.currentFrame = 0;
                else
                    spriteTimer.running = false;
            }
        }
    }
}
