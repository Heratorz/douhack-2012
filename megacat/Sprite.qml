import QtQuick 1.1

Item {
    id: sprite

    signal animationEnded

    property string animationName//: "vxod"
    property int frames//: 6
    property int currentFrame: 0
    property alias running: spriteTimer.running
    property bool repeat: true
    property int repeatCount: -1
    property alias interval: spriteTimer.interval
    property alias imageOpacity: spriteFrame.opacity
    property alias overlayOpacity: spriteOverlay.opacity

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
        smooth: true

        anchors.fill: parent
    }

    Image {
        id: spriteOverlay
        source: "images/sit/overlay.png"
        opacity: 0.0
        anchors.fill: spriteFrame
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
                if (sprite.repeat) {
                    if (sprite.repeatCount !== -1) {
                        if ((--sprite.repeatCount) <= 0) {
//                            spriteTimer.running = false;
                            sprite.animationEnded();
                        }
                    }
                    sprite.currentFrame = 0;
                }
                else {
//                    spriteTimer.running = false;
                    sprite.animationEnded();
                }
            }
        }
    }
}
